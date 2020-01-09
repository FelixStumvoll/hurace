import React from 'react';
import styled from 'styled-components';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faClock,
    faLocationArrow,
    faSkiing,
    faUser,
    faFlagCheckered,
    faCalendarDay
} from '@fortawesome/free-solid-svg-icons';
import { HeaderCard } from '../../shared/HeaderCard';
import { getTime, getDate } from '../../../common/timeConverter';
import { Race } from '../../../models/Race';
import {
    TextBold,
    ColumnFlex,
    WrapText,
    VerticallyAlignedText
} from '../../../theme/CustomComponents';

const ContentRow = styled.div`
    display: grid;
    grid-template-columns: auto 1fr;
    gap: ${props => props.theme.gap};
    width: fit-content;
`;

const DetailGrid = styled.div`
    display: grid;
    grid-template-rows: repeat(5, auto);
    column-gap: 10px;
    row-gap: 5px;
    grid-template-columns: 30px auto;
    width: fit-content;
    height: fit-content;
`;

const CountryText = styled(VerticallyAlignedText)`
    font-style: italic;
    font-weight: normal;
    font-size: 16px;
    color: ${props => props.theme.gray};
    display: flex;
    flex-direction: column;
`;

const HeaderText = styled(VerticallyAlignedText)`
    font-size: 25px;
    font-weight: bold;
`;

const DescriptionHeader = styled(TextBold)`
    margin-bottom: 10px;
`;

const DetailIcon = styled(FontAwesomeIcon)<{ fontSize: number }>`
    font-size: ${props => props.fontSize}px;
    margin: auto;
`;

export const RaceDetailPanel: React.FC<{ race: Race }> = ({ race }) => {
    return (
        <HeaderCard headerText="Stammdaten">
            <ContentRow>
                <DetailGrid>
                    <DetailIcon fontSize={25} icon={faLocationArrow} />
                    <HeaderText>
                        {race?.location.locationName}
                        <CountryText>
                            {' '}
                            {race?.location.country.countryName}
                        </CountryText>{' '}
                    </HeaderText>
                    <DetailIcon fontSize={16} icon={faSkiing} />
                    <VerticallyAlignedText>
                        {race?.discipline.disciplineName}
                    </VerticallyAlignedText>
                    <DetailIcon fontSize={16} icon={faFlagCheckered} />
                    <VerticallyAlignedText>
                        {race?.raceState.raceStateDescription}
                    </VerticallyAlignedText>
                    <DetailIcon fontSize={16} icon={faUser} />
                    <VerticallyAlignedText>
                        {race?.gender.genderDescription}
                    </VerticallyAlignedText>
                    <DetailIcon fontSize={16} icon={faCalendarDay} />
                    <VerticallyAlignedText>
                        {getDate(race?.raceDate)}
                    </VerticallyAlignedText>
                    <DetailIcon fontSize={16} icon={faClock} />
                    <VerticallyAlignedText>
                        {getTime(race?.raceDate)}
                    </VerticallyAlignedText>
                </DetailGrid>

                {race?.raceDescription && (
                    <ColumnFlex>
                        <DescriptionHeader>Beschreibung:</DescriptionHeader>
                        <WrapText>{race?.raceDescription}</WrapText>
                    </ColumnFlex>
                )}
            </ContentRow>
        </HeaderCard>
    );
};
