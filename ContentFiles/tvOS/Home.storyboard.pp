<?xml version="1.0" encoding="UTF-8"?>
<document type="com.apple.InterfaceBuilder.AppleTV.Storyboard" version="3.0" toolsVersion="13771" targetRuntime="AppleTV" propertyAccessControl="none" useAutolayout="YES" colorMatched="YES">
    <device id="appleTV" orientation="landscape">
        <adaptation id="light"/>
    </device>
    <dependencies>
        <deployment identifier="tvOS"/>
        <plugIn identifier="com.apple.InterfaceBuilder.IBCocoaTouchPlugin" version="13772"/>
        <capability name="documents saved in the Xcode 8 format" minToolsVersion="8.0"/>
    </dependencies>
    <scenes>
        <!--Main View-->
        <scene sceneID="8OY-ej-oi3">
            <objects>
                <viewController storyboardIdentifier="HomeView" id="eM6-9P-SPd" customClass="HomeView" sceneMemberID="viewController">
                    <layoutGuides>
                        <viewControllerLayoutGuide type="top" id="jvB-0W-Ryd"/>
                        <viewControllerLayoutGuide type="bottom" id="Uti-bf-D7U"/>
                    </layoutGuides>
                    <view key="view" contentMode="scaleToFill" id="RTx-z9-PEz">
                        <rect key="frame" x="0.0" y="0.0" width="1920" height="1080"/>
                        <autoresizingMask key="autoresizingMask" widthSizable="YES" heightSizable="YES"/>
                        <subviews>
                            <button opaque="NO" contentMode="scaleToFill" fixedFrame="YES" contentHorizontalAlignment="center" contentVerticalAlignment="center" buttonType="roundedRect" lineBreakMode="middleTruncation" translatesAutoresizingMaskIntoConstraints="NO" id="Pku-5W-I90">
                                <rect key="frame" x="871" y="497" width="178" height="86"/>
                                <autoresizingMask key="autoresizingMask" flexibleMaxX="YES" flexibleMaxY="YES"/>
                                <inset key="contentEdgeInsets" minX="40" minY="20" maxX="40" maxY="20"/>
                                <state key="normal" title="Reset"/>
                            </button>
                            <textField opaque="NO" contentMode="scaleToFill" fixedFrame="YES" contentHorizontalAlignment="left" contentVerticalAlignment="center" borderStyle="roundedRect" textAlignment="natural" minimumFontSize="17" translatesAutoresizingMaskIntoConstraints="NO" id="4KU-C8-H93">
                                <rect key="frame" x="146" y="359" width="1700" height="46"/>
                                <autoresizingMask key="autoresizingMask" flexibleMaxX="YES" flexibleMaxY="YES"/>
                                <nil key="textColor"/>
                                <fontDescription key="fontDescription" style="UICTFontTextStyleHeadline"/>
                                <textInputTraits key="textInputTraits"/>
                            </textField>
                        </subviews>
                    </view>
                    <connections>
                        <outlet property="Button" destination="Pku-5W-I90" id="Ydp-au-Otb"/>
                        <outlet property="TextField" destination="4KU-C8-H93" id="x20-Yo-mKY"/>
                    </connections>
                </viewController>
                <placeholder placeholderIdentifier="IBFirstResponder" id="b6r-gh-HMw" userLabel="First Responder" sceneMemberID="firstResponder"/>
            </objects>
            <point key="canvasLocation" x="-1894" y="-2782"/>
        </scene>
    </scenes>
</document>
